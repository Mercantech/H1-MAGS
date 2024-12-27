CREATE TABLE IF NOT EXISTS "User" (
    id VARCHAR(255) PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    last_login TIMESTAMP WITH TIME ZONE,
    is_active BOOLEAN NOT NULL DEFAULT true,
    first_name VARCHAR(50),
    last_name VARCHAR(50)
);

-- Tilføj indeks for hurtigere søgning
CREATE INDEX IF NOT EXISTS idx_user_username ON "User"(username);
CREATE INDEX IF NOT EXISTS idx_user_email ON "User"(email);

-- Tilføj constraints for at sikre data integritet
ALTER TABLE "User" 
    ALTER COLUMN username SET NOT NULL,
    ALTER COLUMN email SET NOT NULL,
    ALTER COLUMN password_hash SET NOT NULL,
    ALTER COLUMN created_at SET NOT NULL,
    ALTER COLUMN updated_at SET NOT NULL,
    ALTER COLUMN is_active SET NOT NULL;

-- Tilføj validering
ALTER TABLE "User" 
    ADD CONSTRAINT email_format CHECK (email ~* '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$'),
    ADD CONSTRAINT username_format CHECK (length(username) >= 3);

-- Tilføj kommentar til tabellen og kolonner
COMMENT ON TABLE "User" IS 'Tabel der indeholder brugeroplysninger';
COMMENT ON COLUMN "User".username IS 'Brugerens unikke brugernavn (min. 3 tegn)';
COMMENT ON COLUMN "User".email IS 'Brugerens unikke email-adresse';
COMMENT ON COLUMN "User".password_hash IS 'Brugerens krypterede adgangskode';
COMMENT ON COLUMN "User".created_at IS 'Tidspunkt for oprettelse af brugeren';
COMMENT ON COLUMN "User".updated_at IS 'Tidspunkt for seneste opdatering';
COMMENT ON COLUMN "User".last_login IS 'Tidspunkt for seneste login';
COMMENT ON COLUMN "User".is_active IS 'Om brugeren er aktiv';
COMMENT ON COLUMN "User".first_name IS 'Brugerens fornavn';
COMMENT ON COLUMN "User".last_name IS 'Brugerens efternavn';
