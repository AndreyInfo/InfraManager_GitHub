using System;
using System.IO;

namespace InfraManager.BLL.ServiceCatalogue.Rules;

public class RuleParameterValueConverter : IRuleParameterConverter, ISelfRegisteredService<IRuleParameterConverter>
{
    public const string VERSION = "2.0";

    private RuleParameterValue ParameterFromBytes(byte[] ruleParameterData)
    {
        if (ruleParameterData == null || ruleParameterData.Length == 0)
            return null;

        using (var ms = new MemoryStream(ruleParameterData))
        using (var br = new BinaryReader(ms))
        {
            var type = (ValueType)br.ReadByte();
            object value;
            //
            switch (type)
            {
                case ValueType.Boolean:
                    value = br.ReadBoolean();
                    break;
                case ValueType.Decimal:
                    value = br.ReadDecimal();
                    break;
                case ValueType.String:
                    value = br.ReadString();
                    break;
                case ValueType.UtcDateTime:
                    value = new DateTime(br.ReadInt64(), DateTimeKind.Utc);
                    break;
                case ValueType.Guid:
                    value = new Guid(br.ReadString());
                    break;
                case ValueType.ClassIDAndGuid:
                    {
                        var classID = br.ReadInt32();
                        var id = new Guid(br.ReadString());
                        //
                        value = Tuple.Create(classID, id);
                    }
                    break;
                case ValueType.TableRow:
                    {
                        var count = br.ReadInt32();
                        var retval = new Tuple<RuleParameterValue[], bool>[count];
                        for (var i = 0; i < count; i++)
                        {
                            var len = br.ReadInt32();
                            var bv_data = br.ReadBytes(len);
                            var binaryValueList = ParametersFromBytes(bv_data);
                            //
                            var isReadOnly = br.ReadBoolean();
                            //
                            retval[i] = Tuple.Create(binaryValueList, isReadOnly);
                        }

                        value = retval;
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }

            //
            return new RuleParameterValue(type, value);
        }
    }

    public RuleParameterValue[] ParametersFromBytes(byte[] ruleParameterData)
    {
        if (ruleParameterData == null || ruleParameterData.Length == 0)
            return new RuleParameterValue[1] { null };

        using (var ms = new MemoryStream(ruleParameterData))
        using (var br = new BinaryReader(ms))
        {
            var isOldVersion = false;
            try
            {
                var ver = br.ReadString();
                isOldVersion = ver != VERSION;
            }
            catch (EndOfStreamException)
            {
                //version 1.0 (empty)
                isOldVersion = true;
            }

            //
            if (isOldVersion)
            {
                var bw = ParameterFromBytes(ruleParameterData);
                return new RuleParameterValue[1] { bw };
            }

            //
            var count = br.ReadInt32();
            var retval = new RuleParameterValue[count];
            for (var i = 0; i < count; i++)
            {
                var bvLength = br.ReadInt32();
                if (bvLength > 0)
                {
                    var bvData = br.ReadBytes(bvLength);
                    retval[i] = ParameterFromBytes(bvData);
                }
                else
                {
                    retval[i] = null;
                }
            }

            //
            return retval;
        }
    }

    public byte[] ParametersToBytes(RuleParameterValue[] parameterValues)
    {
        if (parameterValues == null)
            parameterValues = new RuleParameterValue[1] { null };
        //
        using (MemoryStream ms = new MemoryStream())
        using (BinaryWriter bw = new BinaryWriter(ms))
        {
            bw.Write(VERSION);
            bw.Write(parameterValues.Length);
            //
            for (int i = 0; i < parameterValues.Length; i++)
            {
                var val = parameterValues[i];
                byte[] data = val == null ? Array.Empty<byte>() : GetBytes(val.Type, val.Value);
                //
                bw.Write(data.Length);
                if (data.Length > 0)
                    bw.Write(data);
            }
            //                
            return ms.ToArray();
        }
    }

    private byte[] GetBytes(ValueType type, object value)
    {
        var valueType = value.GetType();
        if (valueType == typeof(double) || valueType == typeof(long)) //небольщой костыль, тк в конвертации используется тип decimal, но после получения значений от фронта value принимает тип double || long, следовательно, конвертация не сработает. В саму конвертацию лезть не стал и не изменил тип decimal на double в контвертации
        {
            value = Convert.ToDecimal(value);
        }

        using (var ms = new MemoryStream())
        using (var bw = new BinaryWriter(ms))
        {
            bw.Write((byte)type); //decimal => long problem
                                  //


            switch (value)
            {
                case bool @bool:
                    bw.Write(@bool);
                    break;

                case decimal @decimal:

                    if (@decimal - Math.Floor(@decimal) == 0)
                        @decimal = Convert.ToDecimal($"{@decimal:F2}");

                    bw.Write(@decimal);

                    break;

                case string @string:

                    bw.Write(@string);

                    break;

                case DateTime dateTime:

                    bw.Write(dateTime.Ticks);

                    break;

                case Guid guid:

                    bw.Write(guid.ToString());

                    break;

                case Tuple<int, Guid> classIDAndGuid:

                    bw.Write(classIDAndGuid.Item1);
                    bw.Write(classIDAndGuid.Item2.ToString());

                    break;

                case Tuple<RuleParameterValue[], bool>[] tableRow:

                    bw.Write(tableRow.Length);

                    for (var i = 0; i < tableRow.Length; i++)
                    {
                        var data = ParametersToBytes(tableRow[i].Item1);

                        bw.Write(data.Length);
                        bw.Write(data);
                        bw.Write(tableRow[i].Item2);
                    }

                    break;

                default:
                    throw new NotSupportedException();
            }

            //                
            return ms.ToArray();
        }
    }
}