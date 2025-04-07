using InfraManager.BLL.Accounts.Tags;
using InfraManager.DAL;
using InfraManager.DAL.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.Accounts;

internal class TagBLL : ITagBLL, ISelfRegisteredService<ITagBLL>
{
    private readonly IRepository<Tag> _repositoryTag;
    private readonly IRepository<UserAccountTag> _repositoryUserAccountTag;
    private readonly IUnitOfWork _saveChangesCommand;

    public TagBLL(IRepository<Tag> repositoryTag,
                    IRepository<UserAccountTag> repositoryUserAccountTag,
                    IUnitOfWork saveChangesCommand)
    {
        _repositoryTag = repositoryTag;
        _repositoryUserAccountTag = repositoryUserAccountTag;
        _saveChangesCommand = saveChangesCommand;
    }

    public async Task<TagDetailsModel> GetByUserAccountIDAsync(int ID, CancellationToken cancellationToken)
    {
        var userAccountTags = await _repositoryUserAccountTag.With(c => c.Tag)
            .ToArrayAsync(c => c.UserAccountID == ID, cancellationToken);

        var tags = new List<Tag>();
        foreach (var item in userAccountTags)
        {
            tags.Add(new Tag(item.TagID, item.Tag.Name));
        }

        return new TagDetailsModel() { UserAccountID = ID, Tags = tags };
    }

    public async Task<TagDetails> CreateAsync(TagData tagData, CancellationToken cancellationToken = default)
    {
        var tagValid = TagValidate(tagData.Tag);
        if (tagValid is not null )
            throw new Exception(tagValid);

        var tag = await _repositoryTag.FirstOrDefaultAsync(x => x.Name.Equals(tagData.Tag), cancellationToken);

        using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
        {
            if (tag is null)
            {
                _repositoryTag.Insert(new Tag(tagData.Tag));
                await _saveChangesCommand.SaveAsync(cancellationToken);
                tag = _repositoryTag.LastOrDefault(x => x.Name.Equals(tagData.Tag));
            }
            else
            {
                var check = await _repositoryUserAccountTag.AnyAsync(x => x.UserAccountID.Equals(tagData.UserAccountID) && x.TagID.Equals(tag.ID), cancellationToken);
                if (check)
                    throw new Exception("Такой тег уже существует для данного пользователя!");
            }

            var userAccountTag = new UserAccountTag() { TagID = tag.ID, UserAccountID = tagData.UserAccountID };

            _repositoryUserAccountTag.Insert(userAccountTag);
            await _saveChangesCommand.SaveAsync(cancellationToken);
            transaction.Complete();
        }

        return new TagDetails(tag.ID, tag.Name);
    }

    public async Task DeleteAsync(TagData tag, CancellationToken cancellationToken = default)
    {
        var entityUserAccountTag = await _repositoryUserAccountTag.FirstOrDefaultAsync(x => x.TagID.Equals(tag.TagID) && x.UserAccountID.Equals(tag.UserAccountID), cancellationToken);

        _repositoryUserAccountTag.Delete(entityUserAccountTag);

        if (!await _repositoryUserAccountTag.AnyAsync(x => x.TagID.Equals(tag.TagID), cancellationToken))
        {
            var entityTag = await _repositoryTag.FirstOrDefaultAsync(x => x.ID.Equals(tag.TagID), cancellationToken);
            _repositoryTag.Delete(entityTag);
        }
        await _saveChangesCommand.SaveAsync(cancellationToken);
    }

    public async Task UpdateAsync(TagDataModel tagData, CancellationToken cancellationToken = default)
    {
        foreach (var item in tagData.Tags)
        {
            var tagValid = TagValidate(item);
            if (tagValid is not null)
                throw new Exception(tagValid);
        }
        
        var userAccountTags = await _repositoryUserAccountTag.ToArrayAsync(c => c.UserAccountID.Equals(tagData.UserAccountID), cancellationToken);

        var tagsRes = await _repositoryTag.ToArrayAsync(c => c.UserAccountTag.Any(), cancellationToken);

        using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
        {
            foreach (var item in userAccountTags)
            {
                _repositoryUserAccountTag.Delete(item);
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);

            foreach (var item in tagsRes)
            {
                if (!await _repositoryUserAccountTag.AnyAsync(x => x.TagID.Equals(item.ID), cancellationToken))
                {
                    _repositoryTag.Delete(item);
                }
            }

            await _saveChangesCommand.SaveAsync(cancellationToken);

            foreach (var item in tagData.Tags)
            {
                if (!await _repositoryTag.AnyAsync(x => x.Name.Equals(item), cancellationToken))
                {
                    _repositoryTag.Insert(new Tag(item));
                }

                await _saveChangesCommand.SaveAsync(cancellationToken);
            }

            var tags = new List<Tag>();
            foreach (var item in tagData.Tags)
            {
                tags.Add(_repositoryTag.LastOrDefault(x => x.Name.Equals(item)));
            }

            foreach (var item in tags)
            {
                var userAccountTag = new UserAccountTag() { TagID = item.ID, UserAccountID = tagData.UserAccountID };
                _repositoryUserAccountTag.Insert(userAccountTag);
            }
            await _saveChangesCommand.SaveAsync(cancellationToken);
            transaction.Complete();
        }
    }

    string TagValidate(string tag)
    {
        if ( string.IsNullOrEmpty(tag))
        {
            return "Пустой тег!";
        }

        if (tag.Length > 50)
        {
            return "Длина тега больше 50-ти символов!";
        }

        string pattern = @"\W+";
        if (Regex.IsMatch(tag, pattern))
        {
            return "В теге недопустимые символы!";
        }

        return null;
    }
}