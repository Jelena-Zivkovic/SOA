using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcService;
using GrpcService.Models;
using MongoDB.Driver;

namespace GrpcService.Services
{
    public class GenerationService : GenerationSer.GenerationSerBase
    {
        private readonly ILogger<GenerationService> _logger;
        private readonly IMongoCollection<GenerationModel> _generation;
        public GenerationService(ILogger<GenerationService> logger)
        {
            _logger = logger;
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("Solar-Power-Generation");
            _generation = database.GetCollection<GenerationModel>("Generation");
        }

        public override async Task<GetAllResponse> GetAll(Empty request, ServerCallContext context)
        {
            var res = await _generation.Find(x => true).ToListAsync();
            var data = res.Select(x => new Generation
            {
                ID = x.ID,
                PlantId = x.Plant_id,
                DateTime = x.Date_time,
                AcPower = x.Ac_power,
                DcPower= x.Dc_power,
                SourceKey = x.Source_key,
                DailyYield = x.Daily_yield,
                TotalYield = x.Total_yield
            }).ToList();
            var response = new GetAllResponse();

            response.Data.AddRange(data);
            return response;
        }
        public override async Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
        {
            var res = await _generation.Find(x => true && x.ID == request.Id).FirstOrDefaultAsync();

            var response = new GetByIdResponse();
            response.Result = res != null ? "Successful" : "There is no data with this id";
            if (response.Result == "Successful")
            {
                response.Data = new Generation
                {
                    ID = response.Data.ID,
                    PlantId = response.Data.PlantId,
                    DateTime = response.Data.DateTime,
                    AcPower = response.Data.AcPower,
                    DcPower = response.Data.DcPower,
                    SourceKey = response.Data.SourceKey,
                    DailyYield = response.Data.DailyYield,
                    TotalYield = response.Data.TotalYield
                };
            }
            return response;
        }
        public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
        {
            var data = new GenerationModel
            {
                Plant_id = request.PlantId,
                Date_time = request.DateTime,
                Ac_power = request.AcPower,
                Dc_power = request.DcPower,
                Source_key = request.SourceKey,
                Daily_yield = request.DailyYield,
                Total_yield = request.TotalYield
            };
            await _generation.InsertOneAsync(data);
            var result = new CreateResponse();
            if (data.ID == null)
            {
                result.Result = "Error while inserting this data";
                return result;
            }
            result.Data = new Generation
            {
                ID = data.ID,
                PlantId = data.Plant_id,
                DateTime = data.Date_time,
                AcPower = data.Ac_power,
                DcPower = data.Dc_power,
                SourceKey = data.Source_key,
                DailyYield = data.Daily_yield,
                TotalYield = data.Total_yield
            };
            result.Result = "Successful";
            return result;
        }
        public override async Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
        { 
            var data = await _generation.Find(x => true && x.ID == request.ID).FirstOrDefaultAsync();
            var result = new UpdateResponse();
            if (data == null)
            {
                result.Result = "There is no data with this Id";
                return result;
            }

             data = new GenerationModel
             {
                 Plant_id = request.PlantId,
                 Date_time = request.DateTime,
                 Ac_power = request.AcPower,
                 Dc_power = request.DcPower,
                 Source_key = request.SourceKey,
                 Daily_yield = request.DailyYield,
                 Total_yield = request.TotalYield
             };
            await _generation.ReplaceOneAsync(x => x.ID == request.ID, data);
            result.Data = new Generation
            {
                ID = data.ID,
                PlantId = data.Plant_id,
                DateTime = data.Date_time,
                AcPower = data.Ac_power,
                DcPower = data.Dc_power,
                SourceKey = data.Source_key,
                DailyYield = data.Daily_yield,
                TotalYield = data.Total_yield
            };
            result.Result = "Successful";
            return result;
        }
        public override async Task<DeleteResponse> Delete(DeleteRequest request, ServerCallContext context)
        {
            var res = await _generation.Find(x => true && x.ID == request.Id).ToListAsync();
            if (res == null)
            {
                return new DeleteResponse { Response = "There is no data with this Id" };
            }
            var r = _generation.DeleteOneAsync(x => x.ID == request.Id);
            return new DeleteResponse { Response = "Successful" };
        }
    }
}
