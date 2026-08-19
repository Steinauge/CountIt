using CountIt.Core.Models;

namespace CountIt.Core.Services;

public interface IStorageService
{
    void Save(List<SectionItem> items);
    List<SectionItem> Load();
}