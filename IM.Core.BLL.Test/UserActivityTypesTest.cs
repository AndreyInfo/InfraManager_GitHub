using InfraManager.BLL.UsersActivityType;
using InfraManager.DAL;
using System.Xml.Linq;
using InfraManager.BLL.UsersActivityType.Obsolete;

namespace IM.Core.BLL.Test;

public class UserActivityTypesTest
{
    #region Получение всех родителей

    #region Получение родителей Test1
    private readonly Guid[] typesID_Test1 = new Guid[]
    {
        Guid.Parse("00000000-0000-0000-0000-000000000000"),
        Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Guid.Parse("00000000-0000-0000-0000-000000000002"),
        Guid.Parse("00000000-0000-0000-0000-000000000003"),
    };

    private const string parentNames_Test1 = "first / second / third / fourth";

    private readonly UserActivityType element = new UserActivityType()
    {
        ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
        ParentID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Name = "first",
        Parent = new UserActivityType()
        {
            ID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            ParentID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Name = "second",
            Parent = new UserActivityType()
            {
                ID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                ParentID = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name = "third",
                Parent = new UserActivityType()
                {
                    ID = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    ParentID = null,
                    Name = "fourth"
                }
            },
        },
    };
    
    [Test]
    public void GetParentsFromTreeByOneBranch_1()
    {
        var details = new UserActivityTypeWithChildsDetails()
        {
            Type = element
        };
        details.BuildParents();

        Assert.AreEqual(4, details.Types.Count());
        Assert.AreEqual(typesID_Test1, details.Types.Select(c=> c.ID));
        Assert.AreEqual(parentNames_Test1, string.Join(" / ", details.Types.Select(c => c.Name).ToArray()));
    }
    #endregion

    #region Получение родилетелей Test2
    UserActivityType typeHas4Parents = new UserActivityType()
    {
        ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
        ParentID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Name = "first",
        Parent = new UserActivityType()
        {
            ID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            ParentID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Name = "second",
            Parent = new UserActivityType()
            {
                ID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                ParentID = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name = "third",
                Parent = new UserActivityType()
                {
                    ID = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    ParentID = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    Name = "fourth",
                    Parent = new UserActivityType()
                    {
                        ID = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                        Name = "fifth",
                        Parent = null
                    }
                }
            }
        },
    };

    private readonly Guid[] typesIDTest2 = new Guid[]
    {
        Guid.Parse("00000000-0000-0000-0000-000000000000"),
        Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Guid.Parse("00000000-0000-0000-0000-000000000002"),
        Guid.Parse("00000000-0000-0000-0000-000000000003"),
        Guid.Parse("00000000-0000-0000-0000-000000000004"),
    };

    private const string parentNamesTest2 = "first / second / third / fourth / fifth";
    [Test]
    public void GetParentsFromTreeByOneBranch_Test2()
    {
        var details = new UserActivityTypeWithChildsDetails()
        {
            Type = typeHas4Parents
        };
        details.BuildParents();

        Assert.AreEqual(5, details.Types.Count());
        Assert.AreEqual(typesIDTest2, details.Types.Select(c => c.ID));
        Assert.AreEqual(parentNamesTest2, string.Join(" / ", details.Types.Select(c => c.Name).ToArray()));
    }
    #endregion
    #endregion

    #region Получение, если один из родителей не инициализрован
    #region Parent is null Test1
    private readonly UserActivityType elementParentNullFirst = new UserActivityType()
    {
        ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
        ParentID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Name = "first",
        Parent = new UserActivityType()
        {
            ID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            ParentID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Name = "second",
            Parent = null
        },
    };


    [Test]
    public void GetParents_IfHasParentID_ButNoInitParent_ResultArgumentNullException_1()
    {
        var details = new UserActivityTypeWithChildsDetails()
        {
            Type = elementParentNullFirst
        };

        Assert.That(() => details.BuildParents(), Throws.ArgumentNullException);
    }
    #endregion

    #region Parent is null Test2
    private readonly UserActivityType elementParentNullSecond = new UserActivityType()
    {
        ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
        ParentID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Name = "first",
        Parent = new UserActivityType()
        {
            ID = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            ParentID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Name = "second",
            Parent = new UserActivityType()
            {
                ID = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                ParentID = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name = "third",
            },
        },
    };


    [Test]
    public void GetParents_IfHasParentID_ButNoInitParent_ResultArgumentNullException_2()
    {
        var details = new UserActivityTypeWithChildsDetails()
        {
            Type = elementParentNullSecond
        };

        Assert.That(() => details.BuildParents(), Throws.ArgumentNullException);
    }
    #endregion
    #endregion
}
