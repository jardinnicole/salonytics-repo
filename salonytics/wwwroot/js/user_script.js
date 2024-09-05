window.scrollTo(0,0);
// header animation
document.addEventListener('DOMContentLoaded', function() {
    const headerColor = document.getElementById('headerColor');

    window.addEventListener('scroll', function() {
        const currentScroll = window.scrollY;

        if (currentScroll === 0) {
            headerColor.style.zIndex = '0';
            headerColor.style.transform = 'translateY(-100%)'; 
        } else {
            headerColor.style.zIndex = '1';
            headerColor.style.transform = 'translateY(0)'; 
        }
    });
});

window.addEventListener('scroll', function() {
    const header = document.querySelector('header');
    const logoLink = document.getElementById('logoLink');

    if (window.scrollY > 50) {  
        header.classList.add('fixed');
    } else {
        header.classList.remove('fixed');
    }
});



//search location box
document.addEventListener('DOMContentLoaded', function() {
    const floatingBox = document.getElementById('floatingBox');
    let isTransformed = false;

    window.addEventListener('scroll', function() {
        if (window.scrollY > 100 && !isTransformed) {
            floatingBox.style.display = 'none'; 
            isTransformed = true;
        } else if (window.scrollY <= 100 && isTransformed) {
            floatingBox.style.display = 'block'; 
            isTransformed = false;
        }
    });
});

//geocoding
document.addEventListener('DOMContentLoaded', function() {
    const searchLocButton = document.getElementById('searchLocButton');
    const useLocationButton = document.getElementById('useLocationButton');
    
    // for cx search by address
    searchLocButton.addEventListener('click', function() {
        const address = document.getElementById('locationInput').value;
        if (address) {
            geocodeAddress(address);
        } else {
            alert("Please enter an address.");
        }
    });
    
    // for cx use current loc
    useLocationButton.addEventListener('click', function() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(successCallback, errorCallback);
        } else {
            alert("Geolocation is not supported by this browser.");
        }
    });
    
    // geocode through API
    function geocodeAddress(address) {
        const apiKey = '66cc18a48cb23307729927rhef00a7c'; 
        const geocodingUrl = `https://geocode.maps.co/search?q=${encodeURIComponent(address)}&api_key=${apiKey}`;
        
        fetch(geocodingUrl)
            .then(response => response.json())
            .then(data => {
                if (data.length > 0) {
                    const userLat = data[0].lat;
                    const userLng = data[0].lon;
                    findNearestBranch(userLat, userLng);
                } else {
                    alert("Address not found. Please enter a valid address.");
                }
            })
            .catch(error => console.error("Error with Geocoding API:", error));
    }
    
    function successCallback(position) {
        const userLat = position.coords.latitude;
        const userLng = position.coords.longitude;
        findNearestBranch(userLat, userLng);
    }

    function errorCallback(error) {
        console.error("Error retrieving location:", error.message);
    }

    // find nearest branch, can modify branch list
    function findNearestBranch(userLat, userLng) {
        const branches = [
            { name: "Apalit Branch", lat: 14.94775, lng: 120.75908 },
            { name: "Bocaue Branch", lat: 14.79747, lng: 120.92928 },
            { name: "Guiguinto Branch", lat: 14.82814, lng: 120.87810 },
            { name: "Malolos Main Branch", lat: 14.84688, lng: 120.81255 },
            { name: "Mojon Branch", lat: 14.86696, lng: 120.82110 },
            { name: "Plaridel Branch", lat: 14.88707, lng: 120.86792 },
            { name: "Pulilan Branch", lat: 14.90096, lng: 120.86759 },
            { name: "San Fernando Pampanga Branch", lat: 15.03801, lng: 120.68229 },
            { name: "South Branch Malolos Branch", lat: 14.86872, lng: 120.80157 },
            { name: "Sta. Rita Guiguinto Branch", lat: 14.84551, lng: 120.85957 }

        ];

        let nearestBranch = null;
        let shortestDistance = Infinity;

        branches.forEach(branch => {
            const distance = haversineDistance(userLat, userLng, branch.lat, branch.lng);
            if (distance < shortestDistance) {
                shortestDistance = distance;
                nearestBranch = branch;
            }
        });

        alert(`The nearest branch is: ${nearestBranch.name}`);
    }

    function haversineDistance(lat1, lon1, lat2, lon2) {
        const toRad = (x) => (x * Math.PI) / 180;
        const R = 6371; 
        const dLat = toRad(lat2 - lat1);
        const dLon = toRad(lon2 - lon1);

        const a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
                  Math.cos(toRad(lat1)) * Math.cos(toRad(lat2)) *
                  Math.sin(dLon / 2) * Math.sin(dLon / 2);
        const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        return R * c; 
    }
});

//carousel
const carousel = document.querySelector('.carousel');
const images = document.querySelectorAll('.carousel img');
let index = 0;

function showImage(index) {
    carousel.style.transform = `translateX(${-index * 100}%)`;
}

function nextImage() {
    index = (index + 1) % images.length;
    showImage(index);
}

setInterval(nextImage, 3000);


