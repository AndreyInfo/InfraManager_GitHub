using InfraManager.BLL.ExpressionBuilder;
using NUnit.Framework;
using System.Linq.Expressions;
using System;
using InfraManager.BLL.ExpressionBuilder.Operators;
using InfraManager.BLL.ExpressionBuilder.Functions;
using InfraManager.BLL.ExpressionBuilder.Constants;
using InfraManager.BLL.ExpressionBuilder.Variables;

namespace InfraManager.CrossPlatform.Common.Test
{
    public class FormulaParameter
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Test() => X + Y;
    }

    public class ExpressionBuilderTests
    {
        #region support methods

        private static ExpressionBuilder<FormulaParameter, int> CreateExpressionBuilder() =>
            new ExpressionBuilder<FormulaParameter, int>(
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
                    new ParameterMethodFunctionExpressionBuilder<FormulaParameter>(nameof(FormulaParameter.Test))
                },                
                new IntegerConstantExpressionBuilder(),
                new[]
                {
                    new PropertyVariableExpressionBuilder<FormulaParameter>(nameof(FormulaParameter.X)),
                    new PropertyVariableExpressionBuilder<FormulaParameter>(nameof(FormulaParameter.Y))
                });

        private static int Execute(
            Expression<Func<FormulaParameter, int>> expression, int x = 1, int y = 1)
        {
            return expression.Compile()(new FormulaParameter { X = x, Y = y });
        }

        #endregion

        #region success scenarios

        [Test]
        public void ConstantFormula()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("25");
            var result = Execute(expression);
            Assert.AreEqual(25, result);
        }

        [Test]
        public void SimpleFormula()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("2+5");
            var result = Execute(expression);
            Assert.AreEqual(7, result);
        }

        [Test]
        public void SimpleFormulaWithSpaces()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build(" 2 + 5 ");
            var result = Execute(expression);
            Assert.AreEqual(7, result);
        }

        [Test]
        public void MultipleOperators()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build(" 2 + 5-3");
            var result = Execute(expression);
            Assert.AreEqual(4, result);
        }

        [Test]
        public void MultipleOperatorsDifferentPriority()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("2 + 5 * 3");
            var result = Execute(expression);
            Assert.AreEqual(17, result);
        }

        [Test]
        public void MultipleOperatorsWithParenthesis()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("( 2 + 5 ) * 3");
            var result = Execute(expression);
            Assert.AreEqual(21, result);
        }

        [Test]
        public void InnerParenthesis()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("( 2 + (5 + 2 - 3)   ) * 3");
            var result = Execute(expression);
            Assert.AreEqual(18, result);
        }

        [Test]
        public void DivisionTest()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("(4 + 4) / 3");
            var result = Execute(expression);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void DivisionReminderTest()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("(4 + 3) % 3");
            var result = Execute(expression);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void Max()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("Max(1,2) - 2");
            var result = Execute(expression);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Min()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("Min(1,2) + 3");
            var result = Execute(expression);
            Assert.AreEqual(4, result);
        }

        [Test]
        public void FunctionCaseInsensitive()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("MAX(1,2) + min(2,3) - mAx(0,1)");
            var result = Execute(expression);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void FunctionArgumentsSpacing()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("Max( 2 , 3 )");
            var result = Execute(expression);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void FunctionOfExpression()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("Max((1+1)*2,1)");
            var result = Execute(expression);
            Assert.AreEqual(4, result);
        }

        [Test]
        public void FunctionOfFunction()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var expression = expressionBuilder.Build("Max(Min(3,4),Max(1,2))");
            var result = Execute(expression);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Variable()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var x = 2;
            var y = 1;
            var expression = expressionBuilder.Build("Max(X+Y,X-Y)");
            var result = Execute(expression, x, y);
            Assert.AreEqual(Math.Max(x + y, x - y), result);
        }

        [Test]
        public void ParameterMethod()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var x = 2;
            var y = 3;
            var expression = expressionBuilder.Build("Test() - X - 1");
            var result = Execute(expression, x, y);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void VariableDivision()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var x = 4;
            var y = 3;
            var expression = expressionBuilder.Build("X%Y");
            var result = Execute(expression, x, y);
            Assert.AreEqual(1, result);
        }

        #endregion

        #region Exception scenarios

        [Test]
        public void MissingOpenParethesis()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build(" 2 + 5 ) * 3"));
            Assert.AreEqual($"FormulaError_{ExpressionValidationException.MissingOpenParenthesis}", ex.MessageKey);
        }

        [Test]
        public void MissingCloseParethesis()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build(" (2 + 5 ) * (3 - 2"));
            Assert.AreEqual($"FormulaError_{ExpressionValidationException.MissingCloseParenthesis}", ex.MessageKey);
        }

        [Test]
        public void MissingLeftArgument()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build(" (2 + 5 ) * (+ 2 - 3)"));
            Assert.AreEqual($"FormulaError_{ExpressionValidationException.MissingLeftArgument}", ex.MessageKey);
        }

        [Test]
        public void MissingRightArgument()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build(" (2 + 5 ) * (2 * (3 + ) )"));
            Assert.AreEqual($"FormulaError_{ExpressionValidationException.MissingRightArgument}", ex.MessageKey);
        }

        [Test]
        public void ZeroDivision()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build("10 / 0"));
            Assert.AreEqual($"FormulaError_{ExpressionValidationException.ZeroDivision}", ex.MessageKey);
        }

        [Test]
        public void InsufficientParameters()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build("Max(1)"));
            Assert.AreEqual($"FormulaError_{ExpressionValidationException.IncorrectParametersQuantity}", ex.MessageKey);
        }

        [Test]
        public void ExtraParameters()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build("Max(1,2,3)"));
            Assert.AreEqual($"FormulaError_{ExpressionValidationException.IncorrectParametersQuantity}", ex.MessageKey);
        }

        [Test]
        public void UnknownStatement()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build("MaxZ(1)"));
            Assert.AreEqual($"FormulaError_{ExpressionValidationException.UnknownStatement}", ex.MessageKey);
        }

        [Test]
        public void NullExpression()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build(null));
            Assert.AreEqual($"FormulaError_{ExpressionValidationException.EmptyExpression}", ex.MessageKey);
        }

        [Test]
        public void EmptyExpression()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build(string.Empty));
            Assert.AreEqual($"FormulaError_{ExpressionValidationException.EmptyExpression}", ex.MessageKey);
        }

        [Test]
        public void WhiteSpacesExpression()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build("   "));
            Assert.AreEqual($"FormulaError_{ExpressionValidationException.EmptyExpression}", ex.MessageKey);
        }

        [Test]
        public void MultupleOperatorsInARow()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build("1*2+++"));
        }

        [Test]
        public void EmptyParameters()
        {
            var expressionBuilder = CreateExpressionBuilder();
            var ex = Assert.Throws<ExpressionValidationException>(() => expressionBuilder.Build("Max( , )"));
            Assert.AreEqual($"FormulaError_{ExpressionValidationException.MissingParameter}", ex.MessageKey);
        }

        #endregion
    }
}
