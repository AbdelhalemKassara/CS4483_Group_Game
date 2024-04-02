public static class GetMapName
{
    public static string MapName(string map = "map_1")
    {
        if (map == "map_1")
        {
            return "Grassy Road";
        }
        else if (map == "map_2")
        {
            return "Rocky Road";
        }
        else if (map == "map_3")
        {
            return "City Road";
        }
        else if (map == "map_4")
        {
            return "Fun Road";
        }
         else if (map == "map_5")
        {
            return "Mountain Road";
        }
        else
        {
            return map;
        }
    }
    
}
