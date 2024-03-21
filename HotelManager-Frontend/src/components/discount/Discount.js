const Discount = ({discount}) => {
    const {id, code, percent, validFrom, validTo } = discount;

    return(
        <div className="discount" key={id}>
            <h2>Code:{code}</h2>
            <p>Percentage:{percent}</p>
            <p>Valid from:{validFrom}</p>
            <p>Valid to:{validTo}</p>
        </div>
    );
};