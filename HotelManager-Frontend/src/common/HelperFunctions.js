const buildQueryString = ({ filter, currentPage, pageSize, sortBy, sortOrder }) => {
    let queryString = '';
    queryString += `?pageNumber=${currentPage}&pageSize=${pageSize}`;
    if (sortBy) {
        queryString += `&sortBy=${sortBy}&sortOrder=${sortOrder}`;
    }
    for (const key in filter) {
        if (filter[key]) {
            queryString += `&${key}=${filter[key]}`;
        }
    }
    return queryString;
}

const formatDate = (dateString) => {
    const options = { day: 'numeric', month: 'numeric', year: 'numeric' };
    const date = new Date(dateString);
    const formattedDate = date.toLocaleDateString('en-GB', options);
    const [day, month, year] = formattedDate.split('/');
    return `${day}.${month}.${year}`;
};

const formatCurrency = (amount) => {
    return new Intl.NumberFormat("de-DE", {
        style: "currency",
        currency: "EUR",
    }).format(amount);
};

const formatReservationDate = (inputDate,hours) => {
    const [day, month, year] = inputDate.split('.');
    const date = new Date(`${year}-${month}-${day}T${hours}:00:00`);
    const timeZoneOffsetMs = date.getTimezoneOffset() * 60000;
    const utcDate = new Date(date.getTime() + timeZoneOffsetMs);
    const formattedDate = utcDate.toISOString();
    return formattedDate;
};

const convertRating = (ratingValue) => {
    let ratingStars = "";
    for (let i = 0; i < ratingValue; i++) {
      ratingStars += "⭐";
    }
    return ratingStars;
  };

export {
    formatReservationDate,
    buildQueryString,
    formatDate,
    formatCurrency,
    convertRating
}