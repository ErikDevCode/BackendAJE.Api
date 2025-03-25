namespace BackEndAje.Api.Application.Census.Commands.CreateCensusAnswer
{
    using Microsoft.AspNetCore.Http;

    public class EstructuraAsset
    {
        public IFormFile? ImageFile { get; set; }

        public string codeAje {get; set;}

        public int branding {get; set;}
    }
}

