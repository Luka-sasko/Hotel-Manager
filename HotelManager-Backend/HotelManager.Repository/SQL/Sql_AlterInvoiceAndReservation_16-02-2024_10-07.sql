DO $$ 
BEGIN
    -- Add InvoiceNumber column to Invoice table if it does not exist
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Invoice' AND column_name = 'InvoiceNumber') THEN
        ALTER TABLE "Invoice"
        ADD COLUMN "InvoiceNumber" CHAR(20) DEFAULT substring(gen_random_uuid()::text from 1 for 20) NOT NULL UNIQUE;
    END IF;

    -- Add ReservationNumber column to Reservation table if it does not exist
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Reservation' AND column_name = 'ReservationNumber') THEN
        ALTER TABLE "Reservation"
        ADD COLUMN "ReservationNumber" CHAR(20) DEFAULT substring(gen_random_uuid()::text from 1 for 20) NOT NULL UNIQUE;
    END IF;

END $$;