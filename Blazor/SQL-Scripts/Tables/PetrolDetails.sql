CREATE TABLE IF NOT EXISTS PetrolDetails (
    id VARCHAR(255) PRIMARY KEY,
    VehicleId VARCHAR(255),
    EngineSize DECIMAL(3,1),
    HorsePower INT,
    Torque DECIMAL(6,2),
    FuelEfficiency DECIMAL(4,2),
    FuelType VARCHAR(20),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(id)
);

CREATE INDEX IF NOT EXISTS idx_petrol_details_vehicle_id ON PetrolDetails(VehicleId);

COMMENT ON TABLE PetrolDetails IS 'Tabel der indeholder detaljer for benzinbiler';
