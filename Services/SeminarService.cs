using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using BC_Api.Interfaces;
using BC_Api.Models;

namespace BC_Api.Services
{
    public class SeminarService(HttpClient httpClient, IConfiguration configuration, Credentials credentials) : ISeminar
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _config = configuration;
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
    }
    public class SeminarData
    {
        public string DocNo { get; set; }
        public string Name { get; set; }
        public decimal Seminar_Duration { get; set; }
        public int Minimum_Participants { get; set; }
        public int Maximum_Participants { get; set; }

    }
}
