import { createContext, useContext, useState } from "react";

const DiscountFilterContext = createContext();

export const DiscountFilterProvider = ({ children }) => {
    const [filter, setFilter] = useState({
        startingValue: 0,
        endValue: 0,
    });

    return (
        <DiscountFilterContext.Provider value={{ filter, setFilter }}>
            {children}
        </DiscountFilterContext.Provider>
    );
}

export const useDiscountFilter = () => {
    const context = useContext(DiscountFilterContext);
    if (!context) {
        throw new Error(
            "useRoomFilter must be used within a RoomFilterProvider"
        );
    }
    return context;
};