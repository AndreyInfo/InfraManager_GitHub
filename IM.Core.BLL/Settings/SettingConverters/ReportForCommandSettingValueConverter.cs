using InfraManager.BLL.Asset;
using InfraManager.BLL.ReportsForCommand;
using System;
using System.Collections.Generic;
using System.IO;

namespace InfraManager.BLL.Settings.SettingConverters;

internal class ReportForCommandSettingValueConverter : SettingValueConverter<List<ReportForCommandData>>,
        ISelfRegisteredService<IConvertSettingValue>,
        ISelfRegisteredService<IConvertSettingValue<List<ReportForCommandData>>>
{
    private const string VERSION = "1.0";

    public override List<ReportForCommandData> Convert(byte[] settingValue)
    {
        if (settingValue.Length == 0)
        {
            return null;
        }

        var list = new List<ReportForCommandData>();

        using (MemoryStream ms = new MemoryStream(settingValue))
        using (BinaryReader br = new BinaryReader(ms))
        {
            string version = br.ReadString();
            if (version != VERSION)
                throw new Exception($"Версия {version} конвертера {nameof(ReportForCommandSettingValueConverter)} устарела");

            int count = br.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                OperationType ot = (OperationType)br.ReadByte();
                Guid repID = new Guid(br.ReadString());

                list.Add(new ReportForCommandData
                {
                    OperationType = ot,
                    ReportID = repID
                });
            }
        }
        return list;
    }

    public override byte[] ConvertBack(List<ReportForCommandData> value)
    {
        using (MemoryStream ms = new MemoryStream())
        using (BinaryWriter bw = new BinaryWriter(ms))
        {
            bw.Write(VERSION);
            bw.Write(value.Count);
            for (int i = 0; i < value.Count; i++)
            {
                var item = value[i];
                bw.Write((byte)item.OperationType);
                bw.Write(item.ReportID.ToString());
            }

            return ms.ToArray();
        }
    }
}