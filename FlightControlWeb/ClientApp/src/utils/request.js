/**
 * 
 * @param {string} url 
 * @param {object} options 
 */
export default async function request(url, options) {
    const response = await fetch(url, options);
    if (!response.ok) {
        throw new Error(await response.text());
    }

    return await response.json();
}
