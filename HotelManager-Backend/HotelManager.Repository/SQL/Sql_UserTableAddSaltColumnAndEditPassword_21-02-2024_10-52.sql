-- Add Salt column if not exists
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'User' AND column_name = 'Salt') THEN
        ALTER TABLE "User"
        ADD COLUMN "Salt" CHAR(44);
    END IF;
END $$;

-- Update existing rows with a default value for Salt
DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'User' AND column_name = 'Salt') THEN
        UPDATE "User" SET "Salt" = 'qYruPSr4PTXVP3bAkgwB5i+2yF2HR1OyVvtl6wHiYiM=';
    END IF;
END $$;

-- Add the column as NOT NULL
DO $$ 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'User' AND column_name = 'Salt' AND is_nullable = 'NO') THEN
        ALTER TABLE "User"
        ALTER COLUMN "Salt" SET NOT NULL;
    END IF;
END $$;

-- Alter Password column type to CHAR(64)
DO $$ 
BEGIN
    IF (SELECT character_maximum_length FROM information_schema.columns WHERE table_name = 'User' AND column_name = 'Password') != 64 THEN
        ALTER TABLE "User" ALTER COLUMN "Password" TYPE CHAR(64);
    END IF;
END $$;

-- Update existing rows with a default value for Password
DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'User' AND column_name = 'Password') THEN
        UPDATE "User" SET "Password" = '7e7461a2c9fb23ae3b3b5112e73e821b0a8be51563f3d24f5b88d29cb7a1c279' WHERE length("Password") < 64;
    END IF;
END $$;