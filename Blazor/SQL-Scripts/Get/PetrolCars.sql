SELECT 
    v.id,
    v.Name,
    v.Description,
    v.Price,
    v.ImageUrl,
    v.Brand,
    v.Model,
    v.Year,
    v.Color,
    v.Mileage,
    p.EngineSize,
    p.HorsePower,
    p.Torque,
    p.FuelEfficiency,
    p.FuelType,
    u.username as SellerUsername,
    u.email as SellerEmail,
    u.first_name as SellerFirstName,
    u.last_name as SellerLastName
FROM Vehicles v
INNER JOIN PetrolDetails p ON v.id = p.VehicleId
INNER JOIN "User" u ON v.SellerId = u.id
WHERE v.VehicleType = 'Petrol'
