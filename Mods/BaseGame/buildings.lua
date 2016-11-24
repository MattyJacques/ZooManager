print('buildings loaded!');

Game:defineBuilding {
    id = "enc_safari",
    name = "Safari Enclosure",
    shop = false,
    enclosure = true,
    cost = 1000,
    maintenance = 10
}

Game:defineBuilding {
    id = "enc_aquarium",
    name = "Aquarium",
    shop = false,
    enclosure = true,
    cost = 3000,
    maintenance = 30
}

Game:defineBuilding {
    id = "shp_hats",
    name = "Fancy Hat Emporium",
    shop = true,
    enclosure = false,
    cost = 500,
    maintenance = 10
}
