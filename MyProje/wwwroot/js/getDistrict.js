function getDistrict() {
    var city = document.getElementById("Citys");
    var cityUrl = "/District/DistrictByCityList?CityId=" + city.value;
    const xhttp = new XMLHttpRequest();

    xhttp.onload = function () { AddDistrict(this); }
    xhttp.open("GET", cityUrl);
    xhttp.send();

}

function AddDistrict(getDistrictList) {

    var District = document.getElementById('DistrictName');
    var DistrictListConvert = JSON.parse(getDistrictList.response);
    console.log(DistrictListConvert);
    District.length = 0;


    DistrictListConvert.forEach(function (district) {

        District.add(new Option(district.districtName,district.id))

    });
}