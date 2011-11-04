using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Service.Abstract;
using eclectica.co.uk.Domain.Abstract;
using eclectica.co.uk.Domain.Entities;
using mab.lib.SimpleMapper;
using eclectica.co.uk.Service.Entities;
using eclectica.co.uk.Service.Extensions;

namespace eclectica.co.uk.Service.Concrete
{
    public class CommentServices : ICommentServices
    {
        private IEntryRepository _entryRepository;
        private ICommentRepository _commentRepository;
        
        public CommentServices(IEntryRepository entryRepository, ICommentRepository commentRepository)
        {
            _entryRepository = entryRepository;
            _commentRepository = commentRepository;
        }

        public IEnumerable<CommentModel> All()
        {
            return Mapper.MapList<Comment, CommentModel>(_commentRepository.All().ToList());
        }

        public CommentModel GetComment(int id)
        {
            return Mapper.Map<Comment, CommentModel>(_commentRepository.Get(id));
        }

        public void AddComment(CommentModel model)
        {
            var comment = new Comment();

            Mapper.CopyProperties<CommentModel, Comment>(model, comment);

            comment.EntryID = model.Entry.EntryID;
            comment.Body = comment.RawBody.Sanitize();

            _commentRepository.Add(comment);

            model.CommentID = comment.CommentID;
        }

        public void UpdateComment(CommentModel model)
        {
            var comment = _commentRepository.Get(model.CommentID);

            Mapper.CopyProperties<CommentModel, Comment>(model, comment);

            _commentRepository.Update(comment);
        }

        public void DeleteComment(int id)
        {
            _commentRepository.Remove(id);
        }
    }
}
