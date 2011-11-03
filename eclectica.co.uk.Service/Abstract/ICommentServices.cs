﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eclectica.co.uk.Service.Entities;

namespace eclectica.co.uk.Service.Abstract
{
    public interface ICommentServices
    {
        IEnumerable<CommentModel> All();
        CommentModel GetComment(int id);
        int AddComment(int entryId, string name, string email, string url, string rawBody);

        void UpdateComment(CommentModel model);
        void DeleteComment(int id);
    }
}
