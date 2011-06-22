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
        private IUnitOfWork _unitOfWork;

        public CommentServices(IEntryRepository entryRepository, ICommentRepository commentRepository, IUnitOfWork unitOfWork)
        {
            _entryRepository = entryRepository;
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CommentModel> All()
        {
            return Mapper.MapList<Comment, CommentModel>(_commentRepository.All().ToList());
        }

        public CommentModel GetComment(int id)
        {
            return Mapper.Map<Comment, CommentModel>(_commentRepository.Get(id));
        }

        public int AddComment(int entryId, string name, string email, string url, string rawBody)
        {
            var entry = _entryRepository.Get(entryId);

            var comment = new Comment
            {
                Name = name,
                Email = email,
                Url = url,
                RawBody = rawBody,
                Body = rawBody.Sanitize(),
                Date = DateTime.Now,
                Approved = true
            };

            entry.Comments.Add(comment);

            _unitOfWork.Commit();

            return comment.CommentID;
        }
    }
}
