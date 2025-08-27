import './App.css';
import React, { useEffect, useState, useCallback, useMemo } from 'react';
import ImageUploader from "./Components/ImageUploader.jsx";

// Simplified normalization with better type safety
const normalizeImage = (raw) => {
    if (!raw || typeof raw !== 'object') return null;

    return {
        id: raw.id || raw.Id || null,
        url: raw.url || raw.Url || raw.filePath || raw.FilePath || null,
        fileName: raw.fileName || raw.FileName || raw.imageName || 'Без названия',
        width: raw.width || raw.Width || null,
        height: raw.height || raw.Height || null,
        size: raw.size || raw.Size || raw.fileSize || null,
    };
};

// Memoized utility function
const formatBytes = (bytes) => {
    if (bytes == null || bytes === 0) return bytes === 0 ? '0 B' : '';

    const units = ['B', 'KB', 'MB', 'GB'];
    const k = 1024;
    const i = Math.floor(Math.log(bytes) / Math.log(k));

    return `${(bytes / Math.pow(k, i)).toFixed(2)} ${units[i]}`;
};

// Custom hook for API operations
const useImageAPI = () => {
    const [images, setImages] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchImages = useCallback(async () => {
        setIsLoading(true);
        setError(null);

        try {
            const response = await fetch('/api/images');

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}: ${response.statusText}`);
            }

            const data = await response.json();
            const normalizedImages = Array.isArray(data)
                ? data.map(normalizeImage).filter(Boolean)
                : [];

            setImages(normalizedImages);
        } catch (err) {
            console.error('Failed to fetch images:', err);
            setError(err.message || 'Неизвестная ошибка при загрузке');
        } finally {
            setIsLoading(false);
        }
    }, []);

    const deleteImage = useCallback(async (imageId) => {
        if (!imageId) return false;

        try {
            const response = await fetch(`/api/images/${imageId}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                throw new Error(`Ошибка удаления: HTTP ${response.status}`);
            }

            // Optimistic update - remove from state immediately
            setImages(prev => prev.filter(img => img.id !== imageId));
            return true;
        } catch (err) {
            console.error('Failed to delete image:', err);
            setError(err.message || 'Ошибка при удалении изображения');
            return false;
        }
    }, []);

    return { images, isLoading, error, fetchImages, deleteImage, setError };
};

// Separate component for better organization
const ImageCard = React.memo(({ image, onDelete }) => {
    const [imageError, setImageError] = useState(false);
    const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);

    const handleDelete = async () => {
        const success = await onDelete(image.id);
        if (success) {
            setShowDeleteConfirm(false);
        }
    };

    //const imageKey = image.id || `${image.url}_${image.fileName}`;

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

// Error boundary component
const ErrorMessage = ({ error, onDismiss }) => (
    <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4 flex justify-between items-center">
        <div>
            <strong>Ошибка:</strong> {error}
        </div>
        {onDismiss && (
            <button
                onClick={onDismiss}
                className="text-red-700 hover:text-red-900 font-bold"
                aria-label="Закрыть сообщение об ошибке"
            >
                ×
            </button>
        )}
    </div>
);

// Loading component
const LoadingSpinner = () => (
    <div className="text-center py-4" role="status" aria-label="Загрузка">
        <div className="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
        <p className="mt-2 text-gray-600">Загрузка изображений...</p>
    </div>
);

// Main component
export default function App() {
    const { images, isLoading, error, fetchImages, deleteImage, setError } = useImageAPI();

    // Load images on mount
    useEffect(() => {
        fetchImages();
    }, [fetchImages]);

    // Memoize handlers to prevent unnecessary re-renders
    const handleUploadSuccess = useCallback(() => {
        fetchImages();
    }, [fetchImages]);

    const handleErrorDismiss = useCallback(() => {
        setError(null);
    }, [setError]);

    // Memoize image grid to prevent unnecessary re-renders
    const imageGrid = useMemo(() => (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {images.map((image) => (
                <ImageCard
                    key={image.id || `${image.url}_${image.fileName}`}
                    image={image}
                    onDelete={deleteImage}
                />
            ))}
        </div>
    ), [images, deleteImage]);

    return (
        <div className="App">
            <div className="container mx-auto p-4">
                <header>
                    <h1 className="text-3xl font-bold text-center mb-8">
                        Галерея изображений
                    </h1>
                </header>

                <section className="mb-8">
                    <h2 className="text-2xl font-semibold mb-4">
                        Загрузка новых изображений
                    </h2>
                    <ImageUploader onUploadSuccess={handleUploadSuccess} />
                </section>

                <section>
                    <div className="flex justify-between items-center mb-4">
                        <h2 className="text-2xl font-semibold">
                            Загруженные изображения ({images.length})
                        </h2>
                        <button
                            onClick={fetchImages}
                            disabled={isLoading}
                            className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700 disabled:opacity-50 transition-colors focus:outline-none focus:ring-2 focus:ring-green-500"
                            aria-label="Обновить список изображений"
                        >
                            {isLoading ? 'Загрузка...' : 'Обновить список'}
                        </button>
                    </div>

                    {isLoading && <LoadingSpinner />}

                    {error && (
                        <ErrorMessage error={error} onDismiss={handleErrorDismiss} />
                    )}

                    {!isLoading && !error && (
                        <>
                            {images.length === 0 ? (
                                <div className="text-center py-8 text-gray-500">
                                    <p className="text-lg">Изображения не найдены</p>
                                    <p className="text-sm">Загрузите первое изображение выше</p>
                                </div>
                            ) : (
                                imageGrid
                            )}
                        </>
                    )}
                </section>
            </div>
        </div>
    );
}
