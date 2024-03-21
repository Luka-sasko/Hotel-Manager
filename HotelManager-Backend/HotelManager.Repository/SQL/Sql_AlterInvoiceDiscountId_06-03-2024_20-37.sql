-- Alter the Invoice table to allow null values in DiscountId
DO $$ 
BEGIN 
  IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'Invoice') THEN
    ALTER TABLE "Invoice" ALTER COLUMN "DiscountId" DROP NOT NULL;
  END IF;
END $$;
