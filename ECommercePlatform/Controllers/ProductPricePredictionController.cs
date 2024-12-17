using Application.AIML;
using Microsoft.AspNetCore.Mvc;

namespace ECommercePlatform.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductPricePredictionController : ControllerBase
    {
        private readonly ProductPricePredictionModel productPricePredictionModel;
        public ProductPricePredictionController()
        {
            productPricePredictionModel = new ProductPricePredictionModel();
            productPricePredictionModel.TrainModel();
        }
        [HttpPost("predict")]
        public ActionResult<float> PredictPrice(ProductData productData){
            var price = productPricePredictionModel.PredictPrice(productData);
            return Ok((float)Math.Round(price, 2));
        }
    }
}
