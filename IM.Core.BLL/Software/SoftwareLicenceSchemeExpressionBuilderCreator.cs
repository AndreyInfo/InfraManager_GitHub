using InfraManager.BLL.EntityFieldEditor;
using InfraManager.BLL.ExpressionBuilder;
using InfraManager.BLL.ExpressionBuilder.Constants;
using InfraManager.BLL.ExpressionBuilder.Functions;
using InfraManager.BLL.ExpressionBuilder.Operators;
using InfraManager.BLL.ExpressionBuilder.Variables;
using InfraManager.DAL.Software;
using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.Software
{
    public static class SoftwareLicenceSchemeExpressionBuilderCreator
    {

        public static string CpuVariableName = "N_CPU";

        public static string CoresVariableName = "N_Cores";

        public static string CoefFunctionName = "Coef";

        public static string CoefFunction = $"{CoefFunctionName}()";

        public static ExpressionBuilder<ILicenceExpressionParameter, int> Create()
        {
            return new ExpressionBuilder<ILicenceExpressionParameter, int>(
                new IBinaryOperatorExpressionBuilder[]
                {
                    new MinusExpressionBuilder(),
                    new MultiplyExpressionBuilder(),
                    new PlusExpressionBuilder(),
                    new DivideExpressionBuilder<int>(),
                    new DivisionReminderExpressionBuilder<int>()
                },
                new[]
                {
                    MaxExpressionBuilder.Create<int>(),
                    MinExpressionBuilder.Create<int>(),
                    new ParameterMethodFunctionExpressionBuilder<ILicenceExpressionParameter>(CoefFunctionName)
                },                
                new IntegerConstantExpressionBuilder(),
                new[]
                {
                    new PropertyVariableExpressionBuilder<ILicenceExpressionParameter>(CpuVariableName, nameof(ILicenceExpressionParameter.CpuQuantity)),
                    new PropertyVariableExpressionBuilder<ILicenceExpressionParameter>(CoresVariableName, nameof(ILicenceExpressionParameter.CoreQuantity))
                });
        }

        public static ExpressionFieldValidator<SoftwareLicenceScheme, ILicenceExpressionParameter, int> CreateValidator(
            Expression<Func<SoftwareLicenceScheme, string>> expressionAccessor)
        {
            return new ExpressionFieldValidator<SoftwareLicenceScheme, ILicenceExpressionParameter, int>(
                Create(),
                expressionAccessor);
        }
    }
}
