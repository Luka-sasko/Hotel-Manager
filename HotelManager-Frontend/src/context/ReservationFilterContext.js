import { createContext, useContext, useState } from "react";

const ReservationFilterContext = createContext();

export const ReservationFilterProvider = ({ children }) => {
    const [filter, setFilter] = useState({
        minPrice: 0,
        maxPrice: 0,
        checkInDate: new Date(),
        checkOutDate: new Date(new Date().getTime() + 24 * 60 * 60 * 1000),
        searchQuery: "",
    });

    return (
        <ReservationFilterContext.Provider value={{ filter, setFilter }}>
            {children}
        </ReservationFilterContext.Provider>
    );
}

export const useReservationFilter = () => {
    const context = useContext(ReservationFilterContext);
    if (!context) {
        throw new Error(
            "useRoomFilter must be used within a RoomFilterProvider"
        );
    }
    return context;
};