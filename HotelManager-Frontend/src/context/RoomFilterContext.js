import { createContext, useContext, useState } from "react";

const RoomFilterContext = createContext();

export const RoomFilterProvider = ({ children }) => {
    const [filter, setFilter] = useState({
        minPrice: 0,
        maxPrice: 0,
        minBeds: 0,
        roomTypeId: null,
        startDate: new Date(),
        endDate: new Date(new Date().getTime() + 24 * 60 * 60 * 1000),
    });

    return (
        <RoomFilterContext.Provider value={{ filter, setFilter }}>
            {children}
        </RoomFilterContext.Provider>
    );
}

export const useRoomFilter = () => {
    const context = useContext(RoomFilterContext);
    if (!context) {
        throw new Error(
            "useRoomFilter must be used within a RoomFilterProvider"
        );
    }
    return context;
};