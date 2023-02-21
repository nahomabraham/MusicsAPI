using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class MusicsController : ControllerBase
{
    DataModel[] data = {
        new DataModel("1", "Nobody", "Casting Crowns", "Gospel"),
        new DataModel("2", "For Tonight", "Giveon", "R&B"),
        new DataModel("3", "Less Like Me", "Jack Williams", "Gospel"),
    };
    [HttpGet]
    public DataModel[] getAllSongs()
    {
        return data;
    }
}