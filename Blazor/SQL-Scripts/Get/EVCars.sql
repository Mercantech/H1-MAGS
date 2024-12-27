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
    ev.BatteryCapacity,
    ev.Range,
    ev.ChargeTime,
    ev.FastCharge,
    u.username as SellerUsername,
    u.email as SellerEmail,
    u.first_name as SellerFirstName,
    u.last_name as SellerLastName
FROM Vehicles v
INNER JOIN EVDetails ev ON v.id = ev.VehicleId
INNER JOIN "User" u ON v.SellerId = u.id
WHERE v.VehicleType = 'EV' 