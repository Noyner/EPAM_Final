using System;
using System.Collections.Generic;
using Epam.Blog.Entities;

namespace Epam.Blog.DAL.Interfaces
{
    public interface ITagDAO
    {
         Tag AddTag(string name);

         IEnumerable<Tag> GetTags(bool orderedById = true);

        int GetTagIdByName(string name);
    }
}
