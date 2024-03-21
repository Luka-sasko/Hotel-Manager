-- Rename "DatedUpdated" to "DateUpdated" in all tables
DO $$ 
BEGIN
    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Role' AND column_name = 'DatedUpdated') THEN
        ALTER TABLE "Role" RENAME COLUMN "DatedUpdated" TO "DateUpdated";
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'User' AND column_name = 'DatedUpdated') THEN
        ALTER TABLE "User" RENAME COLUMN "DatedUpdated" TO "DateUpdated";
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'RoomType' AND column_name = 'DatedUpdated') THEN
        ALTER TABLE "RoomType" RENAME COLUMN "DatedUpdated" TO "DateUpdated";
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Room' AND column_name = 'DatedUpdated') THEN
        ALTER TABLE "Room" RENAME COLUMN "DatedUpdated" TO "DateUpdated";
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Review' AND column_name = 'DatedUpdated') THEN
        ALTER TABLE "Review" RENAME COLUMN "DatedUpdated" TO "DateUpdated";
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Reservation' AND column_name = 'DatedUpdated') THEN
        ALTER TABLE "Reservation" RENAME COLUMN "DatedUpdated" TO "DateUpdated";
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Discount' AND column_name = 'DatedUpdated') THEN
        ALTER TABLE "Discount" RENAME COLUMN "DatedUpdated" TO "DateUpdated";
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Invoice' AND column_name = 'DatedUpdated') THEN
        ALTER TABLE "Invoice" RENAME COLUMN "DatedUpdated" TO "DateUpdated";
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'Service' AND column_name = 'DatedUpdated') THEN
        ALTER TABLE "Service" RENAME COLUMN "DatedUpdated" TO "DateUpdated";
    END IF;

    IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'InvoiceService' AND column_name = 'DatedUpdated') THEN
        ALTER TABLE "InvoiceService" RENAME COLUMN "DatedUpdated" TO "DateUpdated";
    END IF;

END $$;
