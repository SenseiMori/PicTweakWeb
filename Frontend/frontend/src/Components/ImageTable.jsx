import React, { useMemo } from 'react';
import { formatBytes, formatDate } from '../Utils/ImageUtils';

const ImageTable = React.memo(({ images, onDelete, isLoading }) => {
    const tableRows = useMemo(() => (
        images.map((image) => (
            <tr key={image.fileName} className="border-b">
                <td className="p-2">
                    <img src={image.url} alt={image.fileName} className="w-16 h-16 object-cover" loading="lazy" />
                </td>
                <td className="p-2">{image.fileName}</td>
                <td className="p-2">{formatBytes(image.size)}</td>
                <td className="p-2">{formatDate(image.uploadedAt)}</td>
                <td className="p-2">
                    <button onClick={() => onDelete(image.fileName)} className="text-red-500">Удалить</button>
                </td>
            </tr>
        ))
    ), [images, onDelete]);

    if (isLoading) return <p>Загрузка...</p>;
    if (images.length === 0) return <p>Нет изображений</p>;

    return (
        <table className="w-full border-collapse">
            <thead>
                <tr className="bg-gray-200">
                    <th className="p-2">Миниатюра</th>
                    <th className="p-2">Имя</th>
                    <th className="p-2">Размер</th>
                    <th className="p-2">Дата загрузки</th>
                    <th className="p-2">Действия</th>
                </tr>
            </thead>
            <tbody>{tableRows}</tbody>
        </table>
    );
});

export default ImageTable;
