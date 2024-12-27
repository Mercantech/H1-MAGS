CREATE TABLE IF NOT EXISTS EVDetails (
    id VARCHAR(255) PRIMARY KEY,
    VehicleId VARCHAR(255),
    BatteryCapacity DECIMAL(6,2),
    Range DECIMAL(7,2),
    ChargeTime DECIMAL(5,2),
    FastCharge DECIMAL(6,2),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(id)
);

CREATE INDEX IF NOT EXISTS idx_ev_details_vehicle_id ON EVDetails(VehicleId);

COMMENT ON TABLE EVDetails IS 'Tabel der indeholder detaljer for elbiler';
