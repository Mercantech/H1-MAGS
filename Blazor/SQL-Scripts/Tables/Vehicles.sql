CREATE TABLE IF NOT EXISTS Vehicles (
    id VARCHAR(255) PRIMARY KEY,
    Name VARCHAR(100),
    Description TEXT,
    Price DECIMAL(14,2),
    ImageUrl VARCHAR(255),
    CategoryId VARCHAR(255),
    Brand VARCHAR(50),
    Model VARCHAR(50),
    Year INT,
    Color VARCHAR(30),
    Mileage INT,
    SellerId VARCHAR(255),
    CreatedDate TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    ModifiedDate TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    VehicleType VARCHAR(20), -- 'EV' eller 'Petrol'
    FOREIGN KEY (CategoryId) REFERENCES Categories(id),
    FOREIGN KEY (SellerId) REFERENCES "User"(id)
);

CREATE INDEX IF NOT EXISTS idx_vehicles_seller_id ON Vehicles(SellerId);
CREATE INDEX IF NOT EXISTS idx_vehicles_category_id ON Vehicles(CategoryId);
CREATE INDEX IF NOT EXISTS idx_vehicles_vehicle_type ON Vehicles(VehicleType);

COMMENT ON TABLE Vehicles IS 'Tabel der indeholder biler';
