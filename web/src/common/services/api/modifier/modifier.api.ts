import axios from 'axios';



export async function GetAllImages() {
    const response = await axios.get('http://localhost:5022/api/images');
    return response.data;
}