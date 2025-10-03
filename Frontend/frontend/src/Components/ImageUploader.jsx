import React, { useState, useCallback, useEffect } from 'react';
import { handleApiError } from '../Utils/ImageUtils';

const ImageUploader = ({ onUploadSuccess, isVisible }) => {
    const [selectedFile, setSelectedFile] = useState(null);
    const [previewUrl, setPreviewUrl] = useState(null);
    const [isUploading, setIsUploading] = useState(false);
    const [uploadError, setUploadError] = useState(null);

    const handleFileChange = useCallback((event) => {
        const file = event.target.files[0];
        if (file) {
            const url = URL.createObjectURL(file);
            setSelectedFile(file);
            setPreviewUrl(url);
            setUploadError(null);
        }
    }, []);

    // Функция для вычисления ширины/высоты на клиенте
    const getImageDimensions = useCallback((file) => {
        return new Promise((resolve, reject) => {
            const img = new Image();
            img.onload = () => resolve({ width: img.width, height: img.height });
            img.onerror = reject;
            img.src = URL.createObjectURL(file);
        });
    }, []);

    const handleUpload = useCallback(async () => {
        if (!selectedFile) return;

        setIsUploading(true);
        setUploadError(null);

        try {
            // Вычисляем размеры
            const { width, height } = await getImageDimensions(selectedFile);

            const formData = new FormData();
            formData.append('JpegFile', selectedFile); // Файл
            formData.append('FileName', selectedFile.name); // Имя файла (required)
            formData.append('Size', selectedFile.size.toString()); // Размер в байтах (required)
            formData.append('Width', width.toString()); // Ширина
            formData.append('Height', height.toString()); // Высота

            const response = await fetch('/api/images', {
                method: 'POST',
                body: formData,
            });

            if (!response.ok) {
                const errorMessage = await handleApiError(response);
                throw new Error(errorMessage);
            }

            // Парсинг ответа
            let result;
            try {
                result = await response.json();
                console.log('Upload success:', result);
            } catch (parseError) {
                console.error('Failed to parse response:', parseError);
                throw new Error('Некорректный ответ от сервера');
            }

            // Очистка
            setSelectedFile(null);
            if (previewUrl) URL.revokeObjectURL(previewUrl);
            setPreviewUrl(null);

            onUploadSuccess();
        } catch (err) {
            setUploadError(err.message || 'Ошибка загрузки изображения');
        } finally {
            setIsUploading(false);
        }
    }, [selectedFile, previewUrl, onUploadSuccess, getImageDimensions]);

    useEffect(() => {
        return () => {
            if (previewUrl) URL.revokeObjectURL(previewUrl);
        };
    }, [previewUrl]);

    if (!isVisible) return null;

    return (
        <div className="p-4 border rounded bg-white shadow">
            <input
                type="file"
                accept="image/jpeg"
                onChange={handleFileChange}
                className="mb-2 block w-full"
                disabled={isUploading}
            />
            {previewUrl && (
                <img
                    src={previewUrl}
                    alt="Превью изображения"
                    className="w-20 h-20 object-cover mb-2"
                    loading="lazy"
                />
            )}
            <button
                onClick={handleUpload}
                disabled={isUploading || !selectedFile}
                className="bg-blue-500 text-white px-2 py-1 rounded disabled:opacity-50"
            >
                {isUploading ? 'Загрузка...' : 'Загрузить'}
            </button>
            {uploadError && <p className="text-red-500 mt-2">{uploadError}</p>}
        </div>
    );
};

export default React.memo(ImageUploader);
