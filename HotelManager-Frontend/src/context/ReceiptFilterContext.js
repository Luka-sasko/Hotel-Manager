import { createContext, useContext, useState } from "react";

const ReceiptFilterContext = createContext();

export const ReceiptFilterProvider = ({ children }) => {
  const [filter, setFilter] = useState({
    minPrice: 0,
    maxPrice: 0,
    isPaid: "",
    userEmailQuery: "",
  });

  return (
    <ReceiptFilterContext.Provider value={{ filter, setFilter }}>
      {children}
    </ReceiptFilterContext.Provider>
  );
};

export const useReceiptFilter = () => {
  const context = useContext(ReceiptFilterContext);
  if (!context) {
    throw new Error(
      "useReceiptFilter must be used within a ReceiptFilterProvider"
    );
  }
  return context;
};
