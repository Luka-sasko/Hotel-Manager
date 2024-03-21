import React from 'react';
import { formatCurrency } from '../../common/HelperFunctions';

const ServiceList = ({ services }) => {
    const aggregatedServices = services.reduce((accumulator, service) => {
        const existingService = accumulator.find(
            (aggregatedService) => aggregatedService.serviceName === service.serviceName
        );

        if (existingService) {
            existingService.quantity += service.quantity;
            existingService.totalPrice += service.quantity * service.price;
        } else {
            accumulator.push({
                id: service.id,
                serviceName: service.serviceName,
                quantity: service.quantity,
                unitPrice: service.price,
                totalPrice: service.quantity * service.price,
            });
        }

        return accumulator;
    }, []);

    return (
        <>
            <h4>Services:</h4>
            <table className="services-table">
                <thead>
                    <tr>
                        <th>Service</th>
                        <th>Quantity</th>
                        <th>Unit Price</th>
                        <th>Total Price</th>
                    </tr>
                </thead>
                <tbody>
                    {aggregatedServices.map((service) => (
                        <tr key={service.id}>
                            <td>{service.serviceName}</td>
                            <td>{service.quantity}</td>
                            <td>{formatCurrency(service.unitPrice)}</td>
                            <td>{formatCurrency(service.totalPrice)}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </>
    );
}

export default ServiceList;
