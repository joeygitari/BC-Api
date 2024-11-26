using BC_Api.Services;

namespace BC_Api.Interfaces
{
    public interface ISeminar
    {
        Task<dynamic> PostData(SeminarData seminar);
    }
}
