using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using BC_Api.Interfaces;
using BC_Api.Models;
using System.Text.Json;

namespace BC_Api.Services
{
    public class SeminarService(HttpClient httpClient, IConfiguration configuration, Credentials credentials) : ISeminar
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _config = configuration;
        private readonly string serviceRoot = "http://jo:7048/BC240/ODataV4/Company('CRONUS%20International%20Ltd.')";

        public async Task<dynamic> PostData(SeminarData seminar)
        {
            try
            {
                var client = credentials.ObjNav();
                await client.InsertSeminarDataAsync(seminar.DocNo, seminar.Name, seminar.Seminar_Duration, seminar.Minimum_Participants, seminar.Maximum_Participants);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<dynamic> DeleteData(DeleteSeminarData deletedSeminar)
        {
            try
            {
                var client = credentials.ObjNav();
                await client.DeleteSeminarDataAsync(deletedSeminar.DocNo);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<GetSeminarData>> GetSeminarsAsync()
        {
            var url = $"{serviceRoot}/SeminarList";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonData = JsonDocument.Parse(jsonString);
                var seminars = jsonData.RootElement.GetProperty("value");

                return System.Text.Json.JsonSerializer.Deserialize<List<GetSeminarData>>(seminars.ToString());
            }

            return new List<GetSeminarData>();
        }

        public async Task<dynamic> UpdateSeminar(UpdateSeminarData updatedSeminar)
        {
            try
            {
                var client = credentials.ObjNav();
                await client.UpdateSeminarDataAsync(updatedSeminar.DocNo, updatedSeminar.Name, updatedSeminar.Seminar_Duration);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
    public class SeminarData
    {
        public string DocNo { get; set; }
        public string Name { get; set; }
        public decimal Seminar_Duration { get; set; }
        public int Minimum_Participants { get; set; }
        public int Maximum_Participants { get; set; }

    }
    public class GetSeminarData
    {
        public string Name { get; set; }
        public decimal Seminar_Duration { get; set; }
    }
    public class UpdateSeminarData
    {
        public string DocNo { get; set; }
        public string Name { get; set; }
        public decimal Seminar_Duration { get; set; }
    }
    public class DeleteSeminarData
    {
        public string DocNo { get; set; }
    }
}
