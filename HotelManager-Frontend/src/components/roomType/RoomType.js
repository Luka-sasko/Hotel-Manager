const RoomType = ({ roomType }) => {
    const { id, nameRoomType, description } = roomType;

    return (
        <div className="room-type" key={id}>
            <h2>Name of room type:{nameRoomType}</h2>
            <p>Description: {description}</p>
        </div>
    ); 
}; 
