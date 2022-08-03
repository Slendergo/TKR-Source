const express = require('express');
const app = express();
const fs = require('fs');
const config = require("./config.json");
const archiver = require('archiver');
const path = require('path');

app.use(express.json());

app.post("/launcher/requestChanges", (req, res) =>
{
    var values = [];
    for (var a in req.body) {
        values.push(req.body[a]);
    }

    fs.readFile("./CheckSum.json", "utf-8", (err, data) =>
    {
        if (err) {
            res.end();
            return;
        }

        var zip = archiver('zip');

        var json = JSON.parse(data);
        for (var entry in json.Records) {
            if (values.includes(json.Records[entry].CheckSum)) {

                var path = `./${json.Records[entry].Path}`;
                var checkSum = json.Records[entry].CheckSum;
                console.log(path + " " + checkSum);

                var fileExt = path.includes(".") ? path.split('.').pop() : "";
                zip.file(fs.readFileSync(path), { name: `${checkSum}${fileExt}` })
            }
        }

        zip.pipe(res);
        res.attachment('delta.zip').type('zip');
        zip.finalize();
    });
});

app.post("/launcher/checkSum", (req, res) =>
{
    console.log("checkSum");

    fs.readFile("./CheckSum.json", "utf-8", (err, data) =>
    {
        if (err)
        {
            console.error(err);
            res.end();
            return;
        }

        res.send(data);
    });
});

app.post("/launcher/checkVersion", (req, res) =>
{
    res.send(config.version);

    //fs.readFile("./version.json", "utf-8", (err, data) =>
    //{
    //    if (err) {
    //        console.error(err);
    //        res.end();
    //        return;
    //    }

    //    var json = JSON.parse(data);
    //    res.send(json);
    //});
})

app.listen(config.port, () =>
{
    console.log(`LauncherSever app listening on port ${config.port}`);
})