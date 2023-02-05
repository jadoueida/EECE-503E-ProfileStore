using System.Globalization;
using ProfileService.Web.Dtos;

namespace ProfileService.Web.Storage;

public interface IProfileStore
{
    Task UpsertProfile(Profile profile);
    Task SetProfile(Profile profile, string firstname, string lastname);
    Task<Profile?> GetProfile(string username);
}