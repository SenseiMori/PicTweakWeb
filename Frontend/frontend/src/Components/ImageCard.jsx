import React, { useState } from 'react';
import { formatBytes } from '../Utils/ImageUtils';

/**
 * Individual image card component
 * @param {Object} props
 * @param {Object} props.image - Image data object
 * @param {Function} props.onDelete - Delete handler function
 */
const ImageCard = React.memo(({ image, onDelete }) => {
    const [imageError, setImageError] = useState(false);
    const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);

    const handleDelete = async () => {
        const success = await onDelete(image.id);
        if (success) {
            setShowDeleteConfirm(false);
        }
    };

    return (
        <article
            className="border rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow"
            role="img"
            aria-label={`Изображение: ${image.fileName}`}
        >
            <div className="w-full h-40 bg-gray-100 flex items-center justify-center overflow-hidden">
                {image.url && !imageError ? (
                    <a
                        href={image.url}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="w-full h-full flex items-center justify-center"
                        aria-label={`Открыть ${image.fileName} в новой вкладке`}
                    >
                        <img
                            src={image.url}
                            alt={image.fileName}
                            className="max-h-40 object-contain"
                            loading="lazy"
                            onError={() => setImageError(true)}
                        />
                    </a>
                ) : (
                    <div className="text-gray-400" role="img" aria-label="Превью недоступно">
                        Нет превью
                    </div>
                )}
            </div>

            <div className="p-4">
                <h3 className="font-medium text-lg mb-2 truncate" title={image.fileName}>
                    {image.fileName}
                </h3>

                <div className="text-sm text-gray-600 space-y-1">
                    {image.width && image.height && (
                        <p>Размер: {image.width} × {image.height}</p>
                    )}
                    {image.size != null && (
                        <p>Вес: {formatBytes(Number(image.size))}</p>
                    )}
                    {image.id && (
                        <p className="text-xs text-gray-500">ID: {image.id}</p>
                    )}
                </div>

                <div className="mt-4 flex gap-2">
                    {image.url && (
                        <a
                            href={image.url}
                            target="_blank"
                            rel="noopener noreferrer"
                            className="bg-blue-600 text-white px-3 py-1 rounded text-sm hover:bg-blue-700 transition-colors focus:outline-none focus:ring-2 focus:ring-blue-500"
                        >
                            Открыть
                        </a>
                    )}

                    {showDeleteConfirm ? (
                        <div className="flex gap-1">
                            <button
                                onClick={handleDelete}
                                className="bg-red-600 text-white px-2 py-1 rounded text-xs hover:bg-red-700 transition-colors"
                                aria-label="Подтвердить удаление"
                            >
                                Да
                            </button>
                            <button
                                onClick={() => setShowDeleteConfirm(false)}
                                className="bg-gray-600 text-white px-2 py-1 rounded text-xs hover:bg-gray-700 transition-colors"
                                aria-label="Отменить удаление"
                            >
                                Нет
                            </button>
                        </div>
                    ) : (
                        <button
                            onClick={() => setShowDeleteConfirm(true)}
                            className="bg-red-600 text-white px-3 py-1 rounded text-sm hover:bg-red-700 transition-colors focus:outline-none focus:ring-2 focus:ring-red-500"
                            aria-label={`Удалить ${image.fileName}`}
                        >
                            Удалить
                        </button>
                    )}
                </div>
            </div>
        </article>
    );
});

ImageCard.displayName = 'ImageCard';

export default ImageCard;
