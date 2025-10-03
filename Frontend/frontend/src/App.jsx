import React, { useEffect, useCallback } from 'react';
import { useImageAPI } from './Hooks/useImageAPI';
import ImageTable from './Components/ImageTable';
import ControlPanel from './Components/ControlPanel';

export default function App() {
    const { images, isLoading, error, fetchImages, deleteImage } = useImageAPI();

    useEffect(() => {
        fetchImages();
    }, [fetchImages]);

    const handleUploadSuccess = useCallback(() => {
        fetchImages();
    }, [fetchImages]);

    return (
        <div className="min-h-screen bg-gray-100 p-8">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                {/* Левая часть: Таблица изображений */}
                <div className="md:col-span-2 border rounded bg-white shadow p-4">
                    <h2 className="text-xl font-bold mb-4">Изображения</h2>
                    {error && <p className="text-red-500 mb-4">{error}</p>}
                    <ImageTable images={images} onDelete={deleteImage} isLoading={isLoading} />
                </div>

                {/* Правая часть: Блок с кнопками */}
                <div className="md:col-span-1">
                    <ControlPanel onRefresh={fetchImages} onUploadSuccess={handleUploadSuccess} />
                </div>
            </div>
        </div>
    );
}
