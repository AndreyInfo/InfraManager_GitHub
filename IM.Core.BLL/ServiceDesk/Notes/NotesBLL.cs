using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Notes
{
    public class NotesBLL<TNote> : INotesBLL<TNote>
    {
        private readonly IObjectNoteQuery<TNote> _query;
        private readonly IRepository<Note<TNote>> _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ObjectNote> _objectNotes;
        private readonly ILogger<NotesBLL<TNote>> _logger;

        public NotesBLL(
            IObjectNoteQuery<TNote> query,
            IRepository<Note<TNote>> repository,
            IMapper mapper,
            ICurrentUser currentUser,
            IUnitOfWork unitOfWork,
            IRepository<ObjectNote> objectNotes,
            ILogger<NotesBLL<TNote>> logger)
        {
            _query = query;
            _repository = repository;
            _mapper = mapper;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _objectNotes = objectNotes;
            _logger = logger;
        }

        public async Task<NoteDetails[]> GetNotesAsync(Guid parentObjectID, SDNoteType? noteType,
            CancellationToken cancellationToken)
        {
            _logger.LogTrace(
                $"User (ID = {_currentUser.UserId}) requested Notes of {typeof(TNote).Name} for object {parentObjectID} with type {noteType}.");
            var notes = await _query.ExecuteAsync(new ObjectNoteQueryCriteria
            {
                ObjectID = parentObjectID,
                CurrentUserID = _currentUser.UserId,
                NoteType = noteType,
            }
                , cancellationToken);
            _logger.LogTrace(
                $"{notes.Count()} notes of type {typeof(TNote).Name} loaded for  object {parentObjectID} with type {noteType} (user ID = {_currentUser.UserId})");

            return _mapper.Map<NoteDetails[]>(notes);
        }

        public async Task<NoteDetails> GetNoteAsync(Guid parentObjectID, Guid noteID,
            CancellationToken cancellationToken)
        {
            _logger.LogTrace(
                $"User (ID = {_currentUser.UserId}) requested Note of {typeof(TNote).Name} with id {noteID} for object {parentObjectID}.");
            var notes = await _query.ExecuteAsync(
                new ObjectNoteQueryCriteria
                { CurrentUserID = _currentUser.UserId, ObjectID = parentObjectID, NoteID = noteID },
                cancellationToken);
            _logger.LogTrace(
                $"{notes.Count()} {typeof(TNote).Name} found by id {noteID} for object {parentObjectID} (User ID = {_currentUser.UserId}).");

            return _mapper.Map<NoteDetails>(notes.FirstOrDefault());
        }

        public NoteDetails[] GetNotes(Guid parentObjectID, SDNoteType? noteType)
        {
            _logger.LogTrace(
                $"User (ID = {_currentUser.UserId}) requested Notes of {typeof(TNote).Name} for object {parentObjectID} with type {noteType}.");
            var notes = _query.Execute(new ObjectNoteQueryCriteria
            {
                ObjectID = parentObjectID,
                CurrentUserID = _currentUser.UserId,
                NoteType = noteType,
            });
            _logger.LogTrace(
                $"{notes.Count()} notes of type {typeof(TNote).Name} loaded for  object {parentObjectID} with type {noteType} (user ID = {_currentUser.UserId})");
            return _mapper.Map<NoteDetails[]>(notes);
        }

        public async Task<NoteDetails> InsertAsync(InframanagerObject parentObject, NoteData noteData,
            CancellationToken cancellationToken)
        {
            var userID = noteData.UserID == null ? _currentUser.UserId : noteData.UserID.Value;

            _logger.LogTrace(
                $"User (ID = {userID}) request Note created of {typeof(TNote).Name} for object {parentObject}.");
            
            var note = new Note<TNote>(parentObject.Id, userID, noteData.Date);

            _mapper.Map(noteData, note);

            _repository.Insert(note);

            var objectNote = note.CreateObjectNote(userID, true);
            _objectNotes.Insert(objectNote);

            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation(
                $"User (ID = {userID}) successfully created {typeof(TNote).Name} (id = {note.ID})");

            return _mapper.Map<NoteDetails>(
                (await _query.ExecuteAsync(
                    new ObjectNoteQueryCriteria
                    { CurrentUserID = userID, ObjectID = parentObject.Id, NoteID = note.ID },
                    cancellationToken)).FirstOrDefault());
        }

        //TODO заменить первый параметр на PK (Object класс тут не нужен и может только привести к противоречиям)
        public async Task<NoteDetails> UpdateNotesAsync(InframanagerObject parentObject, Guid noteID, NoteData noteData,
            CancellationToken cancellationToken)
        {
            _logger.LogTrace(
                $"User (ID = {_currentUser.UserId}) request Note update of {typeof(TNote).Name} for object {parentObject}.");
            var note = (await _query.ExecuteAsync(
                new ObjectNoteQueryCriteria
                { CurrentUserID = _currentUser.UserId, NoteID = noteID, ObjectID = parentObject.Id },
                cancellationToken)).FirstOrDefault();
            if (note == null)
                throw new ObjectNotFoundException($"note {noteID} for object {parentObject} not found");

            if (noteData.IsRead != null && note.IsRead != noteData.IsRead.Value)
            {
                var objectNote =
                    await _objectNotes.FirstOrDefaultAsync(x => x.ID == noteID && x.UserID == _currentUser.UserId,
                        cancellationToken);
                if (objectNote != null)
                    objectNote.Read = noteData.IsRead.Value;
                else if (noteData.IsRead.Value)
                {
                    var noteEntity = await _repository.FirstOrDefaultAsync(x => x.ID == noteID, cancellationToken);
                    objectNote = noteEntity.CreateObjectNote(_currentUser.UserId, true);
                    _objectNotes.Insert(objectNote);
                }

                await _unitOfWork.SaveAsync(cancellationToken);
                _logger.LogInformation(
                    $"User (ID = {_currentUser.UserId}) successfully updated {typeof(TNote).Name} (id = {noteID})");
                note = (await _query.ExecuteAsync(
                    new ObjectNoteQueryCriteria
                    { CurrentUserID = _currentUser.UserId, NoteID = noteID, ObjectID = parentObject.Id },
                    cancellationToken)).FirstOrDefault();
            }

            return _mapper.Map<NoteDetails>(note);
        }

        public async Task SetAllNotesReadStateAsync(Guid[] objectIDs, bool isRead, CancellationToken cancellationToken)
        {
            _logger.LogTrace(
                $"User (ID = {_currentUser.UserId}) request marking as {(isRead ? "read" : "unread")} all messages for {objectIDs.Length} {typeof(TNote).Name}(s).");

            var objectNoteInfos = await _objectNotes
                    .ToArrayAsync(n => n.UserID == _currentUser.UserId
                        && objectIDs.Contains(n.ObjectID)
                        && n.Read != isRead);

            objectNoteInfos.ForEach(n => n.Read = isRead);

            await _unitOfWork.SaveAsync(cancellationToken);

            _logger.LogInformation(
                $"User (ID = {_currentUser.UserId}) successfully marked as {(isRead ? "read" : "unread")} {objectNoteInfos.Length} messages for {objectIDs.Length} {typeof(TNote).Name}(s).");
        }
    }
}