using System;
using Epam.Blog.BLL.Interfaces;
using Epam.Blog.Entities;
using Epam.Blog.DAL.Interfaces;
using System.Collections.Generic;

namespace Epam.Blog.BLL
{
    public class TagsLogic : ITagsLogic
    {
        public ITagDAO _tagsDAO;

        public TagsLogic(ITagDAO tagsDAO)
        {
            _tagsDAO = tagsDAO;
        }

        public Tag AddTag(string name) =>
            _tagsDAO.AddTag(name);

        public IEnumerable<Tag> GetTags(bool orderedById = true) => _tagsDAO.GetTags(orderedById);

        public int GetTagIdByName(string name) => _tagsDAO.GetTagIdByName(name);
    }
}
