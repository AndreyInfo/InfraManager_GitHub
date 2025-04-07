function Validate(previousValue, value, maxValue) {    
    let maxVal = 1000;
    
    if (maxValue < maxVal) {
        maxVal = maxValue;
    }
    
    //Проверяем, что все символы числа
    const re = /^\d*$/;
    //Предедущие значение не число ставим default
    if (!re.test(previousValue)) {
        return 1;
    }
    let decPreviousValue = parseInt(previousValue, 10);
    let decValue = parseInt(value, 10);
    //Ввели буквы или ноль ставим пред значение 
    if (!re.test(value) || decValue == 0) {
        return decPreviousValue;
    }

    //Ecли ввод меньше макс то можно
    if (decValue <= maxVal) {
        return decValue;
    }

    //Опеределяем последний введеный символов
    const lastSignValue = findLast(String(value));
    let newValue = String(previousValue).slice(0, -1) + lastSignValue;

    //Новое полученое число разница    
    let decNewValue = parseInt(newValue, 10);

    decNewValue = decNewValue > maxVal ? maxVal : decNewValue;

    return decNewValue;

};

function findLast(str) {
    return str.split('')[str.length - 1];
};
