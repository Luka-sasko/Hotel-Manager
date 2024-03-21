import { createContext, useContext, useState } from "react";

const ServicesFilterContext = createContext();

export const ServicesFilterProvider = ({ children }) => {
    const [filter, setFilter] = useState({
        minPrice: 0,
        maxPrice: 0,
        searchQuery: "",
    });

    return (
        <ServicesFilterContext.Provider value={{ filter, setFilter }}>
            {children}
        </ServicesFilterContext.Provider>
    );
}

export const useServicesFilter = () => {
    const context = useContext(ServicesFilterContext);
    if (!context) {
        throw new Error(
            "useRoomFilter must be used within a RoomFilterProvider"
        );
    }
    return context;
};