using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices;

namespace SERVICO_TELEMEDICINA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static List<byte[]> lstImagem = new List<byte[]>();
        private static int QUANTIDADE = 0;
        private static int CONTADOR = 0;

        Dictionary<string, string> openWith =
    new Dictionary<string, string>();

        [HttpGet("ENVIAIMAGEM")]
        public async Task<FileStreamResult> EnviaArquivo()
        {
            if (QUANTIDADE == 0)
                return null;

            if (QUANTIDADE > 0)
            {
                if (CONTADOR == (QUANTIDADE - 1))
                    return null;
            }

            var valor = lstImagem[CONTADOR];

            MemoryStream ms = new MemoryStream(valor);

            CONTADOR++;

            return new FileStreamResult(ms, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream"))
            {
                FileDownloadName = "arquivo.jpg"
            };
        }

        [HttpPost("RECEBEIMAGEM")]
        public async Task<IActionResult> RecebeArquivo([FromForm] IFormFile arquivo)
        {
            if (arquivo.Length > 0)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await arquivo.CopyToAsync(memoryStream);
                        lstImagem.Add(memoryStream.ToArray());
                        QUANTIDADE++;

                        return Ok();
                    }
                }
                catch{}
                {
                    return BadRequest("falha");
                }
            }
            else
            {
                return BadRequest("falha");
            }
        }


        [HttpGet("MEMORIA")]
        public string Memoria()
        {
            long dMemoriaTotal = 0;

            foreach (byte[] x in lstImagem)
            {
                dMemoriaTotal = dMemoriaTotal + x.Length * sizeof(byte);
            }

            return "Quantidade de memória ocupada: " + ConvertBytesToMegabytes(dMemoriaTotal).ToString("0.00"); ;
        }

        private double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }


        [HttpGet("LIMPA")]
        public IActionResult Limpa()
        {
            lstImagem.Clear();

            lstImagem = new List<byte[]>();

            QUANTIDADE = 0;
            CONTADOR   = 0;

            return Ok();
        }

        private void ControlaBufferMemoria()
        { 
        
        }











        //[HttpGet]
        //public async Task<FileStreamResult> EnviaArquivo(string id)
        //{
        //    var stream = await System.IO.File.ReadAllBytesAsync(@"C:\TEMP\jpg\1ac3ed5b-8fb1-47d5-96a5-f25e8913397f.jpg");

        //    MemoryStream ms = new MemoryStream(stream);

        //    return new FileStreamResult(ms, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream"))
        //    {
        //        FileDownloadName = "arquivo.jpg"
        //    };
        //}

        //[HttpPost]
        //public async Task<string> RecebeArquivo([FromForm] IFormFile arquivo)
        //{
        //    if (arquivo.Length > 0)
        //    {
        //        try
        //        {
        //            if (!Directory.Exists(@"c:\temp\entrada\"))
        //            {
        //                Directory.CreateDirectory(@"c:\temp\entrada\");
        //            }

        //            Guid oGuid = Guid.NewGuid();

        //            using (FileStream filestream = System.IO.File.Create(@"c:\temp\entrada\" + oGuid + ".jpg"))
        //            {
        //                await arquivo.CopyToAsync(filestream);


        //                filestream.Flush();
        //                return "ok";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return ex.ToString();
        //        }
        //    }
        //    else
        //    {
        //        return "Ocorreu uma falha no envio do arquivo...";
        //    }
        //}































        //[HttpGet]
        //public async Task<FileResult> RetornaArquivo()
        //{
        //    return new FileStreamResult(new FileStream(@"C:\TEMP\jpg\1ac3ed5b-8fb1-47d5-96a5-f25e8913397f.jpg", FileMode.Open), "application/octet-stream");
        //}

        //[HttpGet]
        //public async Task<IActionResult> Download(string id)
        //{
        //    FileStream stream = new FileStream(@"C:\TEMP\jpg\1ac3ed5b-8fb1-47d5-96a5-f25e8913397f.jpg", FileMode.Open);

        //    await arquivo.CopyToAsync(stream);
        //    filestream.Flush();
        //    return "\\imagens\\" + arquivo.FileName;

        //    if (stream == null)
        //        return NotFound(); // returns a NotFoundResult with Status404NotFound response.

        //    return File(stream, "application/octet-stream"); // returns a FileStreamResult
        //}

        //public static async Task DownloadAsync(Uri requestUri, string filename)
        //{
        //    if (filename == null)
        //        throw new ArgumentNullException("filename");

        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
        //        {
        //            using (Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(), stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, Constants.LargeBufferSize, true))
        //            {
        //                await contentStream.CopyToAsync(stream);
        //            }
        //        }
        //    }
        //}

        //[HttpGet]
        //public async Task<FileResult> retorno(int customerId, string imageName)
        //{
        //    FileStream f = new FileStream(@"C:\TEMP\jpg\1ac3ed5b-8fb1-47d5-96a5-f25e8913397f.jpg", FileMode.Open)

        //    return new FileStreamResult(, "application/octet-stream");
        //}

        //[HttpGet]
        //public async Task<FileStreamResult> Get()
        //{
        //    var stream = await 

        //    return new FileStreamResult(stream, new MediaTypeHeaderValue("text/plain"))
        //    {
        //        FileDownloadName = "README.md"
        //    };
        //}

        //    [HttpGet]
        //    public async Task<IActionResult> Download(string id)
        //    {
        //        Stream stream = await 

        //        if (stream == null)
        //            return NotFound(); // returns a NotFoundResult with Status404NotFound response.

        //        return File(stream, "application/octet-stream"); // returns a FileStreamResult
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> DownloadImage(string id)
        //{
        //    Stream stream = new FileStream(@"C:\TEMP\jpg\1ac3ed5b-8fb1-47d5-96a5-f25e8913397f.jpg", FileMode.Open);
        //    string mimeType = "image/png";
        //    return new FileStreamResult(stream, mimeType)
        //    {
        //        FileDownloadName = "image.png"
        //    };
        //}

        //private static readonly string[] Summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        //private readonly ILogger<WeatherForecastController> _logger;

        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
        //{
        //    _logger = logger;
        //}

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}


        //[HttpGet]
        //public HttpResponseMessage Generate()
        //{
        //    var stream = new MemoryStream();
        //    // processing the stream.

        //    var result = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ByteArrayContent(stream.ToArray())
        //    };

        //    result.Content.Headers.ContentDisposition =
        //        new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //        {
        //            FileName = "CertificationCard.pdf"
        //        };
        //    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        //    return result;
        //}


        //[HttpGet]
        //public FileStreamResult retorno()
        //{
        //    //var getFile = _context.UploadPoliceReports.FirstOrDefault(m => m.UserVehicleID == id);
        //    //var getFile = _context.UploadPoliceReports.where(m => m.UserVehicleID == id);

        //   //byte[] arquivo = FileStream.ReadAllBytes();

        //    FileStream stream = new FileStream(@"C:\TEMP\jpg\1ac3ed5b-8fb1-47d5-96a5-f25e8913397f.jpg", FileMode.Open);

        //    //MemoryStream ms = new MemoryStream(getFile.Data);

        //    return new FileStreamResult(stream, "application/octet-stream");
        //}

        //[HttpGet]
        //public async Task<FileStreamResult> DownloadAsync(string id)
        //{
        //    var stream = awaitGetFileStreamById();

        //    return new FileStreamResult(stream, "application/octet-stream")
        //    {
        //        FileDownloadName = "file.jpg"
        //    };
        //}

        //[HttpGet("{id}")]
        //public async Task<FileResult> GetFileById(int id)
        //{
        //    //provides access to the physical file system, scoping all paths to a directory and its children
        //    IFileProvider provider = new PhysicalFileProvider(BASE_PATH);
        //    var fileInfo = provider.GetFileInfo(lastSavedFilePath);
        //    var fileStream = fileInfo.CreateReadStream();
        //    this._contentTypeProvider.TryGetContentType(lastSavedFilePath, out var mimeType);
        //    return File(fileStream, mimeType, "ProfilePicture.png");
        //}



        //private async FileStream GetFileStreamById()
        //{
        //    FileStream stream = new FileStream(@"C:\TEMP\jpg\1ac3ed5b-8fb1-47d5-96a5-f25e8913397f.jpg", FileMode.Open);

        //    return stream;
        //}

        //[HttpGet]
        //public FileStreamResult VerImagem(int id)
        //{
        //    IFormFile imagemEnviada = arquivos.FirstOrDefault();

        //     MemoryStream ms = new MemoryStream();
        //     imagemEnviada.OpenReadStream().CopyTo(ms);

        //     Image imagem = _context.Imagens.FirstOrDefault(m => m.Id == id);

        //    MemoryStream ms = new MemoryStream(imagem.Dados);
        //    return new FileStreamResult(ms, imagem.ContentType);
        //}

        //[HttpGet]
        //public async Task<FileStreamResult> DownloadAsync(string id)
        //{
        //    var fileName = "myfileName.txt";
        //    var mimeType = "application/....";
        //    var stream = await GetFileStreamById(id);

        //    return new FileStreamResult(stream, mimeType)
        //    {
        //        FileDownloadName = fileName
        //    };
        //}

        //public async Task<FileContentResult> DownloadAsync()
        //{
        //    var fileName = "myfileName.txt";
        //    var mimeType = "application/....";
        //    var fileBytes = await GetFileBytesById(id);

        //    return new FileContentResult(fileBytes, mimeType)
        //    {
        //        FileDownloadName = fileName
        //    };
        //}




        //[HttpPost]
        //public IActionResult Post()
        //{
        //    //var stream = Request.Body;


        //    //byte[] dados;

        //    Guid oGuid = Guid.NewGuid();

        //    //using var image = Image.Load(file.OpenReadStream());
        //    //image.Mutate(x => x.Resize(256, 256));
        //    //image.Save("...");
        //    //return Ok();

        //    //var x = typeof(stream);

        //    //System.IO.File.WriteAllBytes(@"c:\temp\" + oGuid + ".jpg", dados);

        //    //Imagem.Save(@"c:\temp\" + oGuid + ".jpg", ImageFormat.Jpeg);

        //    //System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //    //pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

        //    //byte[] ar = new byte[stream.Length];
        //    //stream.Write(ar, 0, ar.Length);

        //    //System.IO.File.WriteAllBytes(@"c:\temp\entrada\" + oGuid + ".jpg", ar);

        //    return Ok("test");
        //}


    }
}
