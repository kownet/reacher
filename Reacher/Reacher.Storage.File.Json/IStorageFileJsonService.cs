using Reacher.Storage.Data.Models;
using System.Collections.Generic;

namespace Reacher.Storage.File.Json
{
    public interface IStorageFileJsonService
    {
        void Save(Content content);
        IEnumerable<Content> GetAll();
        Content GetLatest();
    }
}